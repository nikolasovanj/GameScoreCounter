import type { ApiPromise } from "../../helpers/api/api";
import type { UserDto } from "../../models/user/UserDto";
import type { Page, PageRequest } from "../../types/Pagination";

export interface IUserApiService {
    Delete(id: string) : ApiPromise
    GetById(id: string) : ApiPromise<UserDto>
    GetByUserName(username: string) : ApiPromise<UserDto>
    GetAll(pageRequest: PageRequest) : ApiPromise<Page<UserDto>>
}