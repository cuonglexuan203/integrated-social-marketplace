import { apiUrl } from "./environment";
const auth = '/identity';
const feed = '/feed';
const chat = '/chat';
export const environment = {
    //Auth Service
    apiAuth: apiUrl + `${auth}/auth`,
    apiUser: apiUrl + `${auth}/user`,

    // Feed Service
    apiFeed: apiUrl + `${feed}/post`,
    // Comment Service

    apiComment: apiUrl + `${feed}/comment`,

    // Chat Service
    apiChat: apiUrl + `${chat}/chat`,
    apiChatHub: apiUrl + `${chat}/chatHub`

}