using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Reports.Enums;

namespace Reports
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; } //unique identifier for the user

        public string? UserName { get; set; } //name of the user

        public string? UserEmail{ get; set; } //email of the user

        //public string? UserPassword { get; set; } //password of the user

        public string? UserPhoneNumber { get; set; } //phone number of the user

        public string? UserAddress { get; set; } //address of the user
        
        public DateTime CreatedAt { get; set; } //date and time of the user creation
    }
}