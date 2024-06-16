namespace StudentReviewManager.PL.VM.Review
{
    public class ReviewVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public bool IsAuthorized { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserId { get; set; }
        public int? CourseId { get; set; }
        public int? SchoolId { get; set; }
        public string Content { get; set; }
    }
}
