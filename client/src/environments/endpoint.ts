import { apiUrl } from "./environment";
const auth = '/identity';
const feed = '/feed';
export const environment = {
    //Auth Service
    apiAuth: apiUrl + `${auth}/auth`,
    apiUser: apiUrl + `${auth}/user`,

    // Feed Service
    apiFeed: apiUrl + `${feed}/post`,
    // Comment Service

    apiComment: apiUrl + `${feed}/comment`,


}