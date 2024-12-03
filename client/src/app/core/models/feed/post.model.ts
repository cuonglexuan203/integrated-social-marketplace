import { prop, required, file } from "@rxweb/reactive-form-validators";

export class CreatePostModel {
    @required()
    userId: string;
    @prop()
    contentText: string;
    @prop()
    @file({maxFiles: 5})
    files: File[] | null;
    @prop()
    tags: string[] | null;
    @prop()
    sharedPostId: string;
}