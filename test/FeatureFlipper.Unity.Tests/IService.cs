namespace FeatureFlipper.Unity.Tests
{
    using System.Collections.Generic;

    public interface IService
    {
        int IntProperty { get; }

        IList<IService> List1 { get; }

        ICollection<IService> Collection1 { get; }

        int GetValue();

        object ObjectMethod();

        void VoidMethod();

        void OutMethod(out int value);

        void OutArrayMethod(out string[] value);

        void OutStringMethod(out string value);

        void OutEnumerableMethod(out IEnumerable<string> value);

        void OutCollectionMethod(out ICollection<string> value);

        IEnumerable<int> IEnumerableMethod();

        string[] ArrayMethod();

        ICollection<bool> CollectionMethod();

        string StringMethod();
    }
}
