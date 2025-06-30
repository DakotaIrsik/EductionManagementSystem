export const RegularExpressions = {
  zipCode: /^[0-9]{5}(?:-[0-9]{4})?$/,
  password: /^^(?=.{6,})(?=.*[a-z])(?=.*[A-Z])(?=.*[@!#$%^&+=]).*$/,
  phoneNumber: /^\d{10}$/,
  email: /[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/
};
