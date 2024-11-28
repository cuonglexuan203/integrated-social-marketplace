import { RoomType } from "../../enums/room-type.enum";
import { Pagination } from "../page/pagination.model";
import { UserResponseModel } from "../user/user.model";
import { Message } from "./message.model";

export class ChatRoom {
    id: string;
    name: string;
    type: RoomType; // Enum (OneToOne, Group)
    participants: UserResponseModel[]; // Array of UserDto
    messagePage: Pagination<Message> | null; // Optional Pagination object of MessageDto
}