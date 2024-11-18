import { AuditableModel } from "../auditable/auditable.model";
import { MediaModel } from "../media/media.model";
import { ReactionModel } from "../reaction/reaction.model";
import { UserResponseModel } from "../user/user.model";

export class FeedPost extends AuditableModel {
    id: string;
    user: UserResponseModel;
    contentText: string;
    media: MediaModel[];
    likesCount: number;
    reactions: ReactionModel[];
    commentsCount: number;
    comments: Comment[];
    link: string | null;
    sharedPostId: string | null;
    tags: string[];
}