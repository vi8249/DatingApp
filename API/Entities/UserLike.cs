namespace API.Entities
{
    public class UserLike
    {
        public AppUser SourceUser { get; set; } // this user is liking other users.
        public int SourceUserId { get; set; }
        public AppUser LikedUser { get; set; }
        public int LikedUserId { get; set; }
    }
}