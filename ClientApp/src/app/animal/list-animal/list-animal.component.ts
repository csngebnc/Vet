import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AnimalService } from 'src/app/_services/animal.service';
import { AddAnimalComponent } from '../add-animal/add-animal.component';

@Component({
  selector: 'app-list-animal',
  templateUrl: './list-animal.component.html',
  styleUrls: ['./list-animal.component.css']
})
export class ListAnimalComponent implements OnInit {

  animals: any = [];  
  page = 1;
  pageSize = 8;

  constructor(private animalService: AnimalService, private modalService: NgbModal) { }

  open() {
    const modalRef = this.modalService.open(AddAnimalComponent);
    modalRef.result.then(() => this.refreshAnimals())
  }

  ngOnInit(): void {
    this.refreshAnimals();
  }

  deleteAnimal(id: number) { 
    this.animalService.deleteAnimal(id).subscribe(() => this.refreshAnimals());
  }

  refreshAnimals(){
    this.animalService.getUserAnimals().subscribe((response) => {
      console.log(response);
      this.animals = response;
    })
  }

}
