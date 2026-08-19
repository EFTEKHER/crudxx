namespace CRUDTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

            //Arrange 
            var math = new MyMath();
            int x = 5;
            int y = 10;
            int expected = 15;

            //Act
            int result = math.Add(x, y);

            
            //Assert 
            Assert.Equal(expected, result);



        }
    }
}
