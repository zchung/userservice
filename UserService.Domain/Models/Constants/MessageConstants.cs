﻿namespace UserService.Domain.Models.Constants
{
    public class MessageConstants
    {
        public const string NullUsersHttpRequest = "Null Users after http request";
        public const string ErrorGettingUsersHttpRequest = "Error getting users for http request";
        public const string RetryErrorHttpRequest = "Error getting users for http request, retrying";
        public const string NoUsersFound = "No users found.";
        public const string NoUserByThatId = "No user by that Id";
    }
}