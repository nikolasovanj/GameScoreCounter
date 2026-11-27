import type { UserDto } from "../../models/UserDtos";

type UserCardProps = {
    userData: UserDto,
};

export default function UserCard ({userData}: UserCardProps) {

    return(
        <div className="flex w-fit items-center bg-mist-500 rounded-2xl px-2 py-3 shadow-xl">
            <div className="grid h-8 w-8 rounded-full border border-gray-200 bg-white p-3 shadow-lg place-content-center text-mist-800 font-bold">
                {userData.username[0]}
            </div>
            <div className="flex-col">
                <div className="flex-1 items-center px-2 font-semibold text-white">
                    <header>{userData.username}</header>
                </div>
                <div className="flex">
                    <div className="px-2 block font-light text-mist-100">
                        <div>Followers: {userData.followers}</div>
                    </div>
                    <div className="px-2 block font-light text-mist-100">
                        <div>Following: {userData.following}</div>
                    </div>
                </div>
            </div >
        </div>
    );
};