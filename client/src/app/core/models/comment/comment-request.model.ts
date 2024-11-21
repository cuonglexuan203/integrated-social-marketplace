import { prop, required } from "@rxweb/reactive-form-validators";
import { AuditableModel } from "../auditable/auditable.model";

export class CommentRequestModel extends AuditableModel {
    @required()
    postId: string;
    @required()
    userId: string;
    @prop()
    commentText: string;
    @prop()
    media: File;
}
