using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SilverLeaf.Common.Constants;
using System;
using System.Collections.Generic;

namespace SilverLeaf.Common.Logging
{
    public static class ExceptionHandler
    {
        public static Dictionary<string, string> ToFriendly(this Exception ex)
        {
            string propertyName = string.Empty;
            var readableError = string.Empty;

            if (ex.GetType() == typeof(DbUpdateException))
            {
                var dbUpdateEx = ex as DbUpdateException;
                var innerMessage = dbUpdateEx.InnerException?.Message;
                if (dbUpdateEx.InnerException?.Message?.Contains(Errors.DuplicateKeyRow) ?? false)
                {
                    //TODO Is there a better way to do this? I hate string manipulation like this...
                    propertyName = innerMessage.Substring(innerMessage.LastIndexOf("_") + 1, innerMessage.LastIndexOf("'") - innerMessage.LastIndexOf("_") - 1);
                    readableError = Errors.KeyExists;
                }
                else if (dbUpdateEx.InnerException.Message?.Contains(Errors.ForeignKeyViolation) ?? false)
                {
                    propertyName = innerMessage.Substring(innerMessage.LastIndexOf("dbo.") + 4, innerMessage.LastIndexOf('"') - innerMessage.LastIndexOf("dbo.") - 4);
                    readableError = Errors.fkViolation;
                }
            }
            else if (ex.GetType() == typeof(AutoMapperConfigurationException))
            {
                propertyName = Errors.AutomapperConfigurationError;
                readableError = ex.Message;
            }

            Dictionary<string, string> response = new Dictionary<string, string>();
            response.Add(propertyName, readableError);
            return response;
        }
    }


}
