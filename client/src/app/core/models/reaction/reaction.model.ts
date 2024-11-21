import { UserResponseModel } from "../user/user.model";

export class ReactionModel {
    user: UserResponseModel;
    type: string;
    createdAt: string;
}

export class ReactionRequestModel {
    postId: string;
    userId: string;
    reactionType: string;

}
