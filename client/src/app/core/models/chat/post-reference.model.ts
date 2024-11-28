import { MediaModel } from "../media/media.model";

export class PostReference {
    id: string;
    contentText: string;
    link: string;
    media: MediaModel[]; // Array of MediaDto
}