using Game.Pandemic.Helpers;

namespace Game.Pandemic.Tests
{
    public class CharacterTests
    {
        [Fact]
        public void TestCircularCollection()
        {
            var col = new CircularCollection<int>(new int[] { 1, 2, 3, 4 });

            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
        }
    }
}
