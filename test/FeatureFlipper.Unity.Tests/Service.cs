namespace FeatureFlipper.Unity.Tests
{
    using System.Collections.Generic;

    public class Service : IService
    {
        private readonly int value;

        public Service()
        {
            this.value = 42;
            this.List1 = new List<IService>();
        }

        public int IntProperty
        {
            get { return this.value; }
        }

        public IList<IService> List1 { get; private set; }

        public ICollection<IService> Collection1 { get; private set; }

        public int GetValue()
        {
            return this.value;
        }

        public object ObjectMethod()
        {
            return new object();
        }

        public void VoidMethod()
        {
            return;
        }

        public void OutMethod(out int value)
        {
            value = this.value;
        }

        public void OutStringMethod(out string value)
        {
            value = this.value.ToString();
        }

        public void OutEnumerableMethod(out IEnumerable<string> value)
        {
            value = new[] { this.value.ToString() };
        }

        public void OutCollectionMethod(out ICollection<string> value)
        {
            value = new[] { this.value.ToString() };
        }

        public IEnumerable<int> IEnumerableMethod()
        {
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
        }

        public string[] ArrayMethod()
        {
            return new[] { "A", "B", "C" };
        }

        public ICollection<bool> CollectionMethod()
        {
            return new[] { true, true, false };
        }

        public void OutArrayMethod(out string[] value)
        {
            value = new[] { "A", "B", "C" };
        }

        public string StringMethod()
        {
            return "A";
        }
    }
}
