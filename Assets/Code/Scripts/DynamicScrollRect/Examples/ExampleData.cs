namespace Project.UI.DynamicScrollRect
{      
    [System.Serializable]
    public class ExampleData
    {
		public int postId;
        public int id;
        public string name;
        public string email;
        public string body;

        public bool fake;

        public ExampleData(bool fake)
        {
            this.fake = fake;
        }
        public ExampleData(int id, bool fake) : this(fake){
            this.id = id;
        }
    }
}