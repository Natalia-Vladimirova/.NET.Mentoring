using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Reflection.Dictionary;

namespace Reflection.Tests
{
    [Subject(nameof(DictionaryBuilder))]
    public class DictionaryBuilderTests
    {
        public class When_creating_dictionary_with_correct_keys_and_values : WithSubject<DictionaryBuilder>
        {
            private static int _instanceCount = 10;
            private static Dictionary<int, TestClass> _result;

            Establish context = () => Subject = new DictionaryBuilder(typeof(int), typeof(TestClass));

            Because of = () =>
            {
                Subject.CreateInstance();

                for (int i = 0; i < _instanceCount; i++)
                {
                    Subject.AddValue(i, new TestClass());
                }

                _result = Subject.Instance as Dictionary<int, TestClass>;
            };

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_contain_five_values = () => _result.Count.ShouldEqual(_instanceCount);

            It should_not_have_null_values = () =>
            {
                foreach (var value in _result)
                {
                    value.Value.ShouldNotBeNull();
                }
            };
        }

        public class When_adding_int_value_to_dictionary_with_int_keys_and_values : WithSubject<DictionaryBuilder>
        {
            private static int _key = 10;
            private static int _value = 5;
            private static Dictionary<int, int> _result;

            Establish context = () => Subject = new DictionaryBuilder(typeof(int), typeof(int));

            Because of = () =>
            {
                Subject.CreateInstance();
                Subject.AddValue(_key, _value);

                _result = Subject.Instance as Dictionary<int, int>;
            };

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_contain_one_value = () => _result.Count.ShouldEqual(1);

            It should_have_key_equals_ten = () => _result.First().Key.ShouldEqual(_key);

            It should_have_value_equals_five = () => _result.First().Value.ShouldEqual(_value);
        }

        public class When_adding_null_value_to_dictionary_with_values_of_reference_type : WithSubject<DictionaryBuilder>
        {
            private static int _key = 10;
            private static Dictionary<int, TestClass> _result;

            Establish context = () => Subject = new DictionaryBuilder(typeof(int), typeof(TestClass));

            Because of = () =>
            {
                Subject.CreateInstance();
                Subject.AddValue(_key, null);

                _result = Subject.Instance as Dictionary<int, TestClass>;
            };

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_contain_one_value = () => _result.Count.ShouldEqual(1);

            It should_have_key_equals_ten = () => _result.First().Key.ShouldEqual(_key);

            It should_have_null_value = () => _result.First().Value.ShouldBeNull();
        }
        
        public class When_adding_null_value_to_dictionary_with_values_of_nullable_type : WithSubject<DictionaryBuilder>
        {
            private static int _key = 10;
            private static Dictionary<int, int?> _result;

            Establish context = () => Subject = new DictionaryBuilder(typeof(int), typeof(int?));

            Because of = () =>
            {
                Subject.CreateInstance();
                Subject.AddValue(_key, null);

                _result = Subject.Instance as Dictionary<int, int?>;
            };

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_contain_one_value = () => _result.Count.ShouldEqual(1);

            It should_have_key_equals_ten = () => _result.First().Key.ShouldEqual(_key);

            It should_have_null_value = () => _result.First().Value.ShouldBeNull();
        }

        public class When_trying_to_add_null_value_to_dictionary_with_values_of_value_type : WithSubject<DictionaryBuilder>
        {
            private static int _key = 10;
            private static Exception _result;

            Establish context = () => Subject = new DictionaryBuilder(typeof(int), typeof(int));

            Because of = () => _result = Catch.Exception(() =>
            {
                Subject.CreateInstance();
                Subject.AddValue(_key, null);
            });

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_be_of_type = () => _result.ShouldBeOfExactType(typeof(ArgumentException));
        }

        public class When_trying_to_add_string_value_to_dictionary_with_int_values : WithSubject<DictionaryBuilder>
        {
            private static int _key = 10;
            private static Exception _result;

            Establish context = () => Subject = new DictionaryBuilder(typeof(int), typeof(int));

            Because of = () => _result = Catch.Exception(() =>
            {
                Subject.CreateInstance();
                Subject.AddValue(_key, "string value");
            });

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_be_of_type = () => _result.ShouldBeOfExactType(typeof(ArgumentException));
        }

        public class When_adding_value_to_dictionary_with_values_of_parent_class : WithSubject<DictionaryBuilder>
        {
            private static int _key = 10;
            private static Dictionary<int, TestClass> _result;

            Establish context = () => Subject = new DictionaryBuilder(typeof(int), typeof(TestClass));

            Because of = () =>
            {
                Subject.CreateInstance();
                Subject.AddValue(_key, new DerivedTestClass());

                _result = Subject.Instance as Dictionary<int, TestClass>;
            };

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_contain_one_value = () => _result.Count.ShouldEqual(1);

            It should_have_value_of_type = () => _result.First().Value.ShouldBeOfExactType(typeof(DerivedTestClass));
        }

        public class When_adding_key_to_dictionary_with_interface_reference_key : WithSubject<DictionaryBuilder>
        {
            private static Dictionary<ITestInterface, string> _result;

            Establish context = () => Subject = new DictionaryBuilder(typeof(ITestInterface), typeof(string));

            Because of = () =>
            {
                Subject.CreateInstance();
                Subject.AddValue(new TestImplementation(), "test");

                _result = Subject.Instance as Dictionary<ITestInterface, string>;
            };

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_contain_one_value = () => _result.Count.ShouldEqual(1);

            It should_be_of_type = () => _result.First().Key.ShouldBeOfExactType(typeof(TestImplementation));
        }

        public class When_trying_to_create_dictionary_with_incorrect_key_type
        {
            private static string _incorrectType = "IncorrectType";

            private static DictionaryBuilder _subject;
            private static Exception _result;

            Because of = () => _result = Catch.Exception(() =>
            {
                _subject = new DictionaryBuilder(_incorrectType, typeof(string).AssemblyQualifiedName);
            });

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_be_of_type = () => _result.ShouldBeOfExactType(typeof(TypeLoadException));
        }

        public class When_trying_to_create_dictionary_with_incorrect_value_type
        {
            private static string _incorrectType = "IncorrectType";

            private static DictionaryBuilder _subject;
            private static Exception _result;

            Because of = () => _result = Catch.Exception(() =>
            {
                _subject = new DictionaryBuilder(typeof(string).AssemblyQualifiedName, _incorrectType);
            });

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_be_of_type = () => _result.ShouldBeOfExactType(typeof(TypeLoadException));
        }

        public class When_trying_to_add_incorrect_value : WithSubject<DictionaryBuilder>
        {
            private static int _key = 10;
            private static Exception _result;

            Establish context = () => Subject = new DictionaryBuilder(typeof(int), typeof(TestClass));

            Because of = () => _result = Catch.Exception(() =>
            {
                Subject.CreateInstance();
                Subject.AddValue(_key, "string param");
            });

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_be_of_type = () => _result.ShouldBeOfExactType(typeof(ArgumentException));
        }
    }
}
