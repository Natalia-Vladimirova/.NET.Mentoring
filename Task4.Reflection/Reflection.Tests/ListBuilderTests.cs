using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Reflection.List;

namespace Reflection.Tests
{
    [Subject(nameof(ListBuilder))]
    public class ListBuilderTests
    {
        public class When_creating_list_with_test_class_values : WithSubject<ListBuilder>
        {
            private static int _instanceCount = 5;
            private static List<TestClass> _result;

            Establish context = () => Subject = new ListBuilder(typeof(TestClass));

            Because of = () =>
            {
                Subject.CreateInstance();

                for (int i = 0; i < _instanceCount; i++)
                {
                    Subject.AddValue(new TestClass());
                }

                _result = Subject.List as List<TestClass>;
            };

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_contain_five_values = () => _result.Count.ShouldEqual(_instanceCount);

            It should_not_have_null_values = () => _result.ForEach(x => x.ShouldNotBeNull());
        }

        public class When_adding_int_value_to_list_with_int_values : WithSubject<ListBuilder>
        {
            private static int _value = 5;
            private static List<int> _result;

            Establish context = () => Subject = new ListBuilder(typeof(int));

            Because of = () =>
            {
                Subject.CreateInstance();
                Subject.AddValue(_value);

                _result = Subject.List as List<int>;
            };

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_contain_one_value = () => _result.Count.ShouldEqual(1);

            It should_equal_five = () => _result.First().ShouldEqual(_value);
        }

        public class When_adding_null_value_to_list_with_values_of_reference_type : WithSubject<ListBuilder>
        {
            private static List<TestClass> _result;

            Establish context = () => Subject = new ListBuilder(typeof(TestClass));

            Because of = () =>
            {
                Subject.CreateInstance();
                Subject.AddValue(null);

                _result = Subject.List as List<TestClass>;
            };

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_contain_one_value = () => _result.Count.ShouldEqual(1);

            It should_have_null_value = () => _result.First().ShouldBeNull();
        }

        public class When_adding_null_value_to_list_with_values_of_nullable_type : WithSubject<ListBuilder>
        {
            private static List<int?> _result;

            Establish context = () => Subject = new ListBuilder(typeof(int?));

            Because of = () =>
            {
                Subject.CreateInstance();
                Subject.AddValue(null);

                _result = Subject.List as List<int?>;
            };

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_contain_one_value = () => _result.Count.ShouldEqual(1);

            It should_have_null_value = () => _result.First().ShouldBeNull();
        }

        public class When_trying_to_add_null_value_to_list_with_values_of_value_type : WithSubject<ListBuilder>
        {
            private static Exception _result;

            Establish context = () => Subject = new ListBuilder(typeof(int));

            Because of = () => _result = Catch.Exception(() =>
            {
                Subject.CreateInstance();
                Subject.AddValue(null);
            });

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_be_of_type = () => _result.ShouldBeOfExactType(typeof(ArgumentException));
        }

        public class When_trying_to_add_string_value_to_list_with_int_values : WithSubject<ListBuilder>
        {
            private static Exception _result;

            Establish context = () => Subject = new ListBuilder(typeof(int));

            Because of = () => _result = Catch.Exception(() =>
            {
                Subject.CreateInstance();
                Subject.AddValue("string value");
            });

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_be_of_type = () => _result.ShouldBeOfExactType(typeof(ArgumentException));
        }

        public class When_adding_value_to_list_with_values_of_parent_class : WithSubject<ListBuilder>
        {
            private static List<TestClass> _result;

            Establish context = () => Subject = new ListBuilder(typeof(TestClass));

            Because of = () =>
            {
                Subject.CreateInstance();
                Subject.AddValue(new DerivedTestClass());

                _result = Subject.List as List<TestClass>;
            };

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_contain_one_value = () => _result.Count.ShouldEqual(1);

            It should_be_of_type = () => _result.First().ShouldBeOfExactType(typeof(DerivedTestClass));
        }

        public class When_adding_value_to_list_with_interface_references : WithSubject<ListBuilder>
        {
            private static List<ITestInterface> _result;

            Establish context = () => Subject = new ListBuilder(typeof(ITestInterface));

            Because of = () =>
            {
                Subject.CreateInstance();
                Subject.AddValue(new TestImplementation());

                _result = Subject.List as List<ITestInterface>;
            };

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_contain_one_value = () => _result.Count.ShouldEqual(1);

            It should_be_of_type = () => _result.First().ShouldBeOfExactType(typeof(TestImplementation));
        }

        public class When_trying_to_create_list_with_incorrect_instance_type
        {
            private static string _incorrectInstanceType = "IncorrectType";

            private static ListBuilder _subject;
            private static Exception _result;

            Because of = () => _result = Catch.Exception(() =>
            {
                _subject = new ListBuilder(_incorrectInstanceType);
            });

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_be_of_type = () => _result.ShouldBeOfExactType(typeof(TypeLoadException));
        }

        public class When_trying_to_add_incorrect_value : WithSubject<ListBuilder>
        {
            private static Exception _result;

            Establish context = () => Subject = new ListBuilder(typeof(TestClass));

            Because of = () => _result = Catch.Exception(() =>
            {
                Subject.CreateInstance();
                Subject.AddValue("string param");
            });

            It should_not_be_null = () => _result.ShouldNotBeNull();

            It should_be_of_type = () => _result.ShouldBeOfExactType(typeof(ArgumentException));
        }
    }
}
