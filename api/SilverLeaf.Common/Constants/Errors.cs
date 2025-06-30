using System;

namespace SilverLeaf.Common.Constants
{
    public static class Errors
    {
        public static string ForeignKeyViolation = "The MERGE statement conflicted with the FOREIGN KEY constraint";
        public static string DuplicateKeyRow = "Cannot insert duplicate key row";
        public static String KeyExists = "already exists";
        public static string fkViolation = "foreign Key violation";
        public static string AutomapperConfigurationError = "AutoMapper Configuration";
    }
}
