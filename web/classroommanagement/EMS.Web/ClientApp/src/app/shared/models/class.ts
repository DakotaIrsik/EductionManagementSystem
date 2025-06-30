export class Class {
    Id: number;
    CourseId: number;
    Lesson: number;
    Week: number;
    Session: string;
    Card: string;
    Fictionality: string;
    Genre: string;
    Title: string;
    Classwork: string;
    Homework: string;
    LessonPlan: string;
    Flipchart: string;
    Material: string;
    AllDocuments: string;
    Phonics: string;
    TargetReadingSkill: string;

}

export class StudioCardDecks {

    constructor(classes: Class[]) {
        this.cards = classes;
        this.count = classes.length;
    }
    public cards: Class[];
    public count: number;
}
