import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};


@Injectable({
  providedIn: 'root'
})
export class AnimalService {
  baseUrl: string = 'https://localhost:44345/api/'

  constructor(private http: HttpClient) { }

  addAnimal(model: FormData) {
    return this.http.post(this.baseUrl + 'animal/addanimal', model);
  }

  updateAnimal(model: any) {
    return this.http.put(this.baseUrl + 'animal/updateanimal', model);
  }

  updatePhoto(model: FormData) {
    return this.http.put(this.baseUrl + 'animal/updatephoto', model);
  }

  changeStateOfAnimal(id) {
    return this.http.put(this.baseUrl + 'animal/archiveAnimal/' + id, null);
  }

  deletePhoto(id: any) {
    return this.http.delete(this.baseUrl + 'animal/deletephoto/' + id);
  }

  deleteAnimal(id: number) {
    return this.http.delete(this.baseUrl + 'animal/' + id);
  }

  getAnimal(id: any) {
    return this.http.get(this.baseUrl + 'animal/get/' + id);
  }

  getUserAnimals() {
    return this.http.get(this.baseUrl + 'animal/my-animals');
  }

  getUserAnimalsByEmail(email) {
    return this.http.get(this.baseUrl + 'animal/by-email/' + email);
  }

  getUserAnimalsByUserId(id) {
    return this.http.get(this.baseUrl + 'animal/by-id/' + id);
  }

  getUserArchivedAnimals() {
    return this.http.get(this.baseUrl + 'animal/my-archived-animals');
  }

  getArchivedAnimalsByUserId(id) {
    return this.http.get(this.baseUrl + 'animal/archived-by-id/' + id);
  }


}
