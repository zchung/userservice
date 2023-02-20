namespace UserService.Domain.Models
{
    public interface IUserGenderNumberData
    {
        public int Age { get; }
        public int Female { get; }
        public int Male { get; }
    }

    public class UserGenderNumberData : IUserGenderNumberData
    {
        public int Age { get; }
        public int Female { get; }
        public int Male { get; }

        public UserGenderNumberData(int age, int female, int male)
        {
            Age = age;
            Female = female;
            Male = male;
        }

        public override string ToString()
        {
            return $"Age: {Age} Female: {Female} Male: {Male}";
        }
    }
}