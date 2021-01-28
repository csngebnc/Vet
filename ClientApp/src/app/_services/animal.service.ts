import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
  })
};


@Injectable({
  providedIn: 'root'
})
export class AnimalService {

  constructor(private http: HttpClient) { }

  addAnimal(model: FormData){
    return this.http.post('https://localhost:44345/api/animal/addanimal', model);
  }

  updateAnimal(model: any){
    return this.http.put('https://localhost:44345/api/animal/updateanimal', model);
  }

  updatePhoto(model: FormData){
    return this.http.put('https://localhost:44345/api/animal/updatephoto', model);
  }

  deletePhoto(id: any){
    return this.http.delete('https://localhost:44345/api/animal/deletephoto/'+id);
  }

  deleteAnimal(id: number){
    return this.http.delete('https://localhost:44345/api/animal/'+id);
  }

  getAnimal(id: any){
    return this.http.get('https://localhost:44345/api/animal/get/'+id);
  }

  getUserAnimals(){
    return this.http.get('https://localhost:44345/api/animal/my-animals');
  }

}
