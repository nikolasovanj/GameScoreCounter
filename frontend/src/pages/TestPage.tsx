import UserCard from "../components/user/UserCard";
import type { UserDto } from "../models/UserDtos";

const testUser: UserDto = {
    username: "Djura", 
    email: "djura@mail.com",  
    followers: 34, 
    following: 45
};

export default function TestPage(){
  return (<div><UserCard userData={testUser}/></div>);
}