import { MoreDialogModel } from "../models/more/more-dialog.model";

export const MoreDialogData: MoreDialogModel[] = [
    {
        name: 'Add to Bookmarks',
        action: 'addToBookmarks',
    },
    {
        name: 'Report',
        action: 'report',
    },
    {
        name: 'Not interesting',
        action: 'notInteresting',
    },
]

export const MoreDialogStateFilter: MoreDialogModel[] = [
    {
        name: 'Remove this saved post',
        action: 'removeSavedPost',
    },
    {
        name: 'Report',
        action: 'report',
    },
    {
        name: 'Not interesting',
        action: 'notInteresting',
    },
]