import { prop, required } from "@rxweb/reactive-form-validators";

export class MockDataPost {
    @prop()
    id: number;
    userId: number;
    name: string;
    type: string;
    description: string;
    @required()
    mediaUrl: MockDataMedia [];
    likesCount: MockDataLike [];
    commentsCount: MockDataComment [];
    sharesCount: MockDataShare [];
    createdAt: string;
    updatedAt: string;
    isDeleted: boolean;
    deletedAt: string;
    @required()
    price: number;
}

export class MockDataComment {
    id: number;
    postId: number;
    userId: number;
    commentText: string;
    createdAt: string;
    updatedAt: string;
    isDeleted: boolean;
    deletedAt: string;
    parentCommentId: number;
}

export class MockDataLike {
    id: number;
    postId: number;
    userId: number;
    createdAt: string;
    updatedAt: string;
    isDeleted: boolean;
    deletedAt: string;
}

export class MockDataShare {
    id: number;
    postId: number;
    userId: number;
    createdAt: string;
    updatedAt: string;
    isDeleted: boolean;
    deletedAt: string;
}

export class MockDataMedia {
    imageUrl: string [];
    videoUrl: string [];
}