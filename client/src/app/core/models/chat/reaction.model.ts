import { ReactionType } from "../../enums/reaction-type.enum";
import { UserResponseModel } from "../user/user.model";

export class Reaction {
    user: UserResponseModel; // Instance of UserDto
    type: ReactionType; // Enum (Like, Love, Haha, etc.)
    createdAt: string | null; // Optional date as a string
}