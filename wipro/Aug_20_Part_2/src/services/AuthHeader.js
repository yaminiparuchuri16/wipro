const AuthHeader = () => {
  
    const token = localStorage.getItem("token")
    if (token) {
        alert("Success")
       return { Authorization: 'Bearer ' + token }; // for Spring Boot back-end
    } else {
      return {};
}
}
export default AuthHeader