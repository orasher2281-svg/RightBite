using Core.Models;
using Core.Repository;
using Core.Resource;
using Core.Services;

namespace Server.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Add(User entity)
        {
            return await _userRepository.Add(entity);
        }

        public async Task<int> DeleteById(int id)
        {
            return await _userRepository.DeleteById(id);
        }

        public async Task<User?> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }
        public async Task<List<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }


        public async Task<User?> Update(User entity)
        {
            return await _userRepository.Update(entity);
        }
        public async Task<int> Register(User u)
        {
            var user = await _userRepository.GetByEmail(u.Email);
            if (user != null)
            {
                throw new Exception("מייל זה כבר קיים במערכת");
            }
            string securePassword = BCrypt.Net.BCrypt.HashPassword(u.Password);
            u.Password = securePassword;
            CalculateUserNutritionGoals(u);
            await _userRepository.Add(u);
            return u.Id;

        }
        
        

        public async Task<int> Login(LoginResource loginResource)
        {
            var user = await _userRepository.GetByEmail(loginResource.Email);
            if (user == null)
            {
                throw new KeyNotFoundException("המשתמש אינו קיים במערכת");
            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginResource.Password, user.Password);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("סיסמה שגויה");
            }
            return user.Id;

        }
       
        public void CalculateUserNutritionGoals(User user)
        {
            double bmr;

            // 1. חישוב BMR לפי נוסחת Mifflin-St Jeor
            if (user.Gender == UserGender.Male)
            {
                bmr = (10 * user.Weight) + (6.25 * user.Height) - (5 * user.Age) + 5;
            }
            else
            {
                bmr = (10 * user.Weight) + (6.25 * user.Height) - (5 * user.Age) - 161;
            }

            // 2. חישוב TDEE (הוצאה אנרגטית יומית כוללת - מקדם 1.2 לאורח חיים רגיל)
            double tdee = bmr * 1.2;

            // 3. התאמה לפי מטרת המשתמש (Goal)
            double targetCalories = user.Goal switch
            {
                UserGoal.Lose => tdee - 500,
                UserGoal.Gain => tdee + 500,
                UserGoal.Maintain => tdee,
                _ => tdee
            };

            // 4. עדכון שדות האובייקט (לפי חלוקת מאקרו: 30% חלבון, 30% שומן, 40% פחמימות)
            user.DailyCalories = (int)Math.Round(targetCalories);

            // חישוב בגרמים: חלבון/פחמימה = 4 קל' לגרם, שומן = 9 קל' לגרם
            user.TargetProtein = Math.Round((targetCalories * 0.30) / 4, 1);
            user.TargetFat = Math.Round((targetCalories * 0.30) / 9, 1);
            user.TargetCarbs = Math.Round((targetCalories * 0.40) / 4, 1);
        }


    }
}
