import { prop, required } from "@rxweb/reactive-form-validators";

export class MockDataPost {
    @prop()
    id: number;
    name: string;
    type: string;
    description: string;
    @required()
    mediaUrl: string;
    likesCount: number;
    commentsCount: number;
    createdAt: string;
    updatedAt: string;
    isDeleted: boolean;
    deletedAt: string;
    @required()
    price: number;
}