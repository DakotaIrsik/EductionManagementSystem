import { Paging } from 'src/app/shared/models/api/paging';

export class ClassSearchRequest extends Paging  {
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
    Phonics: string;
}
