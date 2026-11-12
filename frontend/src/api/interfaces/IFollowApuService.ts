import type { ApiPromise } from "../../helpers/api/api";
import type { UserDto } from "../../models/UserDtos";
import type { Page, PageRequest } from "../../types/Pagination";

export interface IFollowApiService {
    follow(id: string) : ApiPromise;
    unfollow(id: string) : ApiPromise;
    getFollowers(request: PageRequest) : ApiPromise<Page<UserDto>>;
    getFollowing(request: PageRequest) : ApiPromise<Page<UserDto>>;
}