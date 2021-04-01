import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AnimalDto } from 'src/app/_models/animaldto';
import { AnimalService } from 'src/app/_services/animal.service';
import { AddAnimalComponent } from '../add-animal/add-animal.component';

@Component({
  selector: 'app-list-archived-animals',
  templateUrl: './list-archived-animals.component.html',
  styleUrls: ['./list-archived-animals.component.css']
})
export class ListArchivedAnimalsComponent implements OnInit {

  animals: AnimalDto[] = [];
  page = 1;
  pageSize = 8;

  constructor(private animalService: AnimalService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.refreshAnimals();
  }

  deleteAnimal(id: number) {
    if (confirm('Biztosan szeretnéd törölni?')) {
      this.animalService.deleteAnimal(id).subscribe(() => this.refreshAnimals());
    }
  }

  archiveAnimal(id) {
    if (confirm('Biztosan szeretnéd visszaállítani?')) {
      this.animalService.changeStateOfAnimal(id).subscribe(() => this.refreshAnimals());
    }
  }

  refreshAnimals() {
    this.animalService.getUserArchivedAnimals().subscribe((animals: AnimalDto[]) => {
      this.animals = animals;
    })
  }

}
