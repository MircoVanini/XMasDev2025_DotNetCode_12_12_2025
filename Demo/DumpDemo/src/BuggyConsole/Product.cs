namespace BuggyConsole
{
    public class Product
    {
        string name;
        int id;
        char[] details = new char[10_000];

        public Product(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
