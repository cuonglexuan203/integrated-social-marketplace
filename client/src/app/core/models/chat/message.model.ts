import { MessageStatus } from "../../enums/message-status.enum";
import { MediaModel } from "../media/media.model";
import { MessageReadInfo } from "./message-read-info.model";
import { PostReference } from "./post-reference.model";
import { Reaction } from "./reaction.model";
import { AuditableModel } from "../auditable/auditable.model";

export class Message extends AuditableModel {
    id: string;
    roomId: string;
    senderId: string;
    contentText: string;
    media: MediaModel[]; // Array of MediaDto
    reactions: Reaction[]; // Array of ReactionDto
    messageReadInfo: MessageReadInfo[]; // Array of MessageReadInfoDto
    attachedPosts: PostReference[]; // Array of PostReferenceDto
    status: MessageStatus; // Enum (Pending, Sending, etc.)
    
}