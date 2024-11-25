import { AuditableModel } from "../auditable/auditable.model";
import { MediaModel } from "../media/media.model";
import { ReactionModel } from "../reaction/reaction.model";
import { UserResponseModel } from "../user/user.model";

export class Comment extends AuditableModel {
    id: string;
    postId: string;
    user: UserResponseModel;
    media: MediaModel[];
    commentText: string;
    likesCount: number;
    reactions: ReactionModel[];
    parentCommentId: string | null;
}
