import { prop, required, file } from "@rxweb/reactive-form-validators";

export class CreatePostModel {
    @required()
    userId: string;
    @required()
    contentText: string;
    @required()
    @file({maxFiles: 5})
    files: File[] | null;
    @required()
    tags: string[];
}