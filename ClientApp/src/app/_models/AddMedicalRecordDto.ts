import { TherapiaOnMedicalRecord } from "./therapiarecorddto";

export interface AddMedicalRecordDto {
    date: Date;

    ownerEmail: string;

    animalId: number | null;

    anamnesis: string;
    symptoma: string;
    details: string;

    therapias: TherapiaOnMedicalRecord[];

}