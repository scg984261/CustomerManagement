using CustomerManagement.Command;

namespace CustomerManagement.Test.Command
{
    public class TestDelegateCommand
    {
        private int? firstNumber;
        private int? secondNumber;
        private int? result;

        [SetUp]
        public void Setup()
        {
            this.firstNumber = 1;
            this.secondNumber = 2;
            this.result = 0;
        }

        public void Add(object? parameter)
        {
            result = firstNumber + secondNumber;
        }

        public bool CanAdd(object? parameter)
        {
            return this.firstNumber != null && this.secondNumber != null;
        }

        [Test]
        public void TestConstructor_CanExecuteNotNull()
        {
            DelegateCommand testDelegateCommand = new DelegateCommand(this.Add, this.CanAdd);

            Assert.That(testDelegateCommand.execute, Is.Not.Null);
            Assert.That(testDelegateCommand.canExecute, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_CanExecuteIsNull()
        {
            DelegateCommand testDelegateCommand = new DelegateCommand(this.Add);

            Assert.That(testDelegateCommand.execute, Is.Not.Null);
            Assert.That(testDelegateCommand.canExecute, Is.Null);
            Assert.That(testDelegateCommand.CanExecute(new object()), Is.True);
        }

        [Test]
        public void TestExecute_ShouldExecuteSuccessfully()
        {
            DelegateCommand testDelegateCommand = new DelegateCommand(this.Add, this.CanAdd);

            testDelegateCommand.Execute(new object());

            const int expectedResult = 3;

            Assert.That(this.result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void TestCanExecute_ShouldReturnTrue()
        {
            DelegateCommand testDelegateCommand = new DelegateCommand(this.Add, this.CanAdd);

            Assert.That(testDelegateCommand.CanExecute(new object()), Is.True);
        }

        [Test]
        public void TestCanExecute_ShouldReturnFalse()
        {
            this.firstNumber = null;

            DelegateCommand testDelegateCommand = new DelegateCommand(this.Add, this.CanAdd);

            Assert.That(testDelegateCommand.CanExecute(new object()), Is.False);
        }
    }
}
