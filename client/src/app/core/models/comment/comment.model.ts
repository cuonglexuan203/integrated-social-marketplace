import { AuditableModel } from "../auditable/auditable.model";
import { MediaModel } from "../media/media.model";
import { ReactionModel } from "../reaction/reaction.model";

export class Comment extends AuditableModel {
    id: string;
    postId: string;
    userId: string;
    username: string;
    media: MediaModel[];
    commentText: string;
    likesCount: number;
    reactions: ReactionModel[];
    parentCommentId: string | null;
}