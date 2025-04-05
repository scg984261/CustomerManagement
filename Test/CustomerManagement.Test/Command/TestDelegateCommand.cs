using CustomerManagement.Command;

namespace CustomerManagement.Test.Command
{
    public class TestDelegateCommand
    {
        private int? firstNumber = 1;
        private int? secondNumber = 2;
        private int? result = 0;

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

            this.firstNumber = 1;
        }
    }
}
