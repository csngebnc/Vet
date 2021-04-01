export interface VaccineRecordDto {
    id: number;
    date: Date;

    vaccineId: number;
    vaccineName: string;

    animalId: number;
    animalName: string;
}