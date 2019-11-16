namespace TimiShared.Identity {

    public class User {
        private int _id;

        public int Id {
            get {
                return this._id;
            }
        }

        public User() {
        }

        public User(int id) {
            this._id = id;
        }
    }
}