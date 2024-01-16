import React, { Children, useState } from "react";
// import { ChevronUpIcon } from "@heroicons/react/24/solid";
import { UserCircleIcon } from "@heroicons/react/24/outline";
import { ShoppingCartIcon } from "@heroicons/react/24/outline";
import { CurrencyDollarIcon } from "@heroicons/react/24/outline";
import { LanguageIcon } from "@heroicons/react/24/outline";
import { MapPinIcon } from "@heroicons/react/24/outline";
// import { Bars4Icon } from "@heroicons/react/24/outline";
import { CubeTransparentIcon } from "@heroicons/react/24/outline";
import 'swiper/css/grid';

import { useRef } from 'react';
import { Swiper, SwiperSlide } from 'swiper/react';
import 'swiper/css';
import 'swiper/css/pagination';
import 'swiper/css/navigation';
import '../src/style.css';

import { Pagination, Navigation, Grid } from 'swiper/modules';

import s1 from "../src/assets/swaiper/1.jpg"
import s2 from "../src/assets/swaiper/2.jpg"
import s3 from "../src/assets/swaiper/3.webp"
import s4 from "../src/assets/swaiper/4.webp"
import s5 from "../src/assets/swaiper/5.jpg"
import s6 from "../src/assets/swaiper/6.jpg"
import s7 from "../src/assets/swaiper/7.jpg"
import s8 from "../src/assets/swaiper/8.jpg"
import s9 from "../src/assets/swaiper/9.jpg"
import s10 from "../src/assets/swaiper/10.webp"
import logo from "../src/assets/swaiper/logo.gif"

import { StarIcon } from "@heroicons/react/24/outline";




import {
  Menu,
  MenuHandler,
  MenuList,
  MenuItem,
  Button,
  Input,
} from "@material-tailwind/react";
import Catalog from "./components/Catalog/Catalog";

const App = () => {

 let Catalogs = [
    {
      catalogName : "Недвижимость",
      children : [
        {
          name: "1-Categorie 1",
          children: [
            "1-1 subcatagorie1",
            "1-1 subcatagorie2",
            "1-1 subcatagorie3",
          ]
        },
        {
          name: "1-Categorie 2",
          children: [
            "1-2 subcatagorie3",
            "1-2 subcatagorie2",
            "1-3 subcatagorie3",
          ]
        },
        {
          name: "1-Categorie 3",
          children: [
            "1-3 subcatagorie1",
            "1-3 subcatagorie2",
            "1-3 subcatagorie3",
          ]
        },
        {
          name: "1-Categorie 4",
          children: [
            "1-4 subcatagorie1",
            "1-4 subcatagorie2",
            "1-4 subcatagorie3",
          ]
        }        
      ]
    },
    {
      catalogName : "Траспорт",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },
    {
      catalogName : "Личный Вещи",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },
    {
      catalogName : "Продукты питания",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },
    {
      catalogName : "Техника и электроника",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },
    {
      catalogName : "Для дома и дачи",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },
    {
      catalogName : "Хобби спорт и Отдых",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },
    {
      catalogName : "Медицина здаровыие и гигиена",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },

    {
      catalogName : "Красота",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },

    {
      catalogName : "Строительство и ремонт",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },
    {
      catalogName : "Животныей и растении",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },
    {
      catalogName : "Путишествие",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },
    {
      catalogName : "Все для Бизнеса",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },
    {
      catalogName : "Работа вакация",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },

    {
      catalogName : "Услуги",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },

    {
      catalogName : "Ислам",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },

    {
      catalogName : "Наши товары",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },
    




    {
      catalogName : "Военные",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },



    {
      catalogName : "Акции",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },
    {
      catalogName : "Другие товары",
      children : [
        {
          name: "2-Categorie 1",
          children: [
            "2-1 subcatagorie 1",
            "2-1 subcatagorie 2",
            "2-1 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 2",
          children: [
            "2-2 subcatagorie 3",
            "2-2 subcatagorie 2",
            "2-2 subcatagorie 3",
          ]
        },
        {
          name: "2-Categorie 3",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        },
        {
          name: "2-Categorie 4",
          children: [
            "2-2 subcatagorie1",
            "2-2 subcatagorie2",
            "2-2 subcatagorie3",
          ]
        }        
      ]
    },

  ]


  return (
    <div>
      <div className="w-[100%] #009000 flex justify-around shadow-md h-[40px] mb-[0px]">
        <ul className="flex justify-evenly w-[100%]">
          <li className="flex items-center items-center">
            <StarIcon className="w-[40px] h-[25px]" color="gold" />
            Доступный цены</li>
          <li className="flex items-center">
          <StarIcon className="w-[40px] h-[25px]" color="gold"  />
            Быстый Доставка</li>
          <li className="flex items-center">
          <StarIcon className="w-[40px] h-[25px]" color="gold"  />
            Застрохованный Доставка</li>
          <li className="flex items-center">
          <StarIcon className="w-[40px] h-[25px]" color="gold"  />
            Эксперты в этой сфере</li>
          <li className="flex items-center"> 
          <StarIcon className="w-[40px] h-[25px]" color="gold"  />
            Огромный выбор</li>
        </ul>   
      </div>    
      <div className="bg-[#009900] h-[60px] w-[95%] shadow-2xl rounded-[7px] m-auto flex">
      <div className="flex items-center ml-[15px]">

    <Menu  dismiss={{
        itemPress: false,
      }} >
      <MenuHandler>
        <Button className="w-[60px] h-[40px] text-[8px] text-[white] bg-[#008000] p-[0px]">
          <CubeTransparentIcon className="w-[25px] h-[25px] m-auto" color='gold'/>
          Catalog</Button>
      </MenuHandler>
      <MenuList>
      <Input
          label="Search"
          containerProps={{
            className: "mb-4",
          }}
        />
      <Catalog  obj={Catalogs}/>
      </MenuList>
    </Menu>

        <div className="ml-[20px] flex text-[#000000] text-[32px]">
        <img className="w-[150px] h-[30px]" preload="auto" loop autoplay src="src/assets/swaiper/logo.gif" />
        </div>

      </div>
    

      <div className="mt-[0px] flex items-center w-[400px] ml-[50px]">
      <Input
        type="search"
        placeholder="Search"
        className="!border h-[50px] !border-gray-300 bg-white text-gray-900 shadow-lg shadow-gray-900/5 ring-4 ring-transparent placeholder:text-gray-500 focus:!border-gray-900 focus:!border-t-gray-900 focus:ring-gray-900/10"
        labelProps={{
          // className: "hidden",
        }}
        // label="Search"
        containerProps={{ className: "min-w-[100px]" }}
      />
      </div>

      <div className="w-[300px] items-center flex justify-between ml-[35px]">
      <Button className="bg-[#008000] w-[40px] h-[40px] border-[#007000] border-[1px]">
        <LanguageIcon strokeWidth={2.5}  className={`h-5 w-5 ml-[-6px] ml-[-9px] mt-[-2px]`} color="gold" />
        </Button>
        <Button className="bg-[#008000]  w-[40px] h-[40px] border-[#007000] border-[1px]">
        <MapPinIcon strokeWidth={2.5}  className={`h-5 w-5 ml-[-6px] ml-[-9px] mt-[-2px]`} color="gold" />
        </Button>
        <Button className="bg-[#008000]  w-[40px] h-[40px] border-[#007000] border-[1px]">
        <CurrencyDollarIcon strokeWidth={2.5}  className={`h-5 w-5 ml-[-9px] mt-[-2px]`}color="gold" />
        </Button>
        <Button  className="bg-[#008000]  w-[40px] h-[40px] border-[#007000] border-[1px]">
        <UserCircleIcon color="gold" strokeWidth={2.5}  className={`h-5 w-5 ml-[-9px] mt-[-2px]`}/>
        </Button>
        <Button className="bg-[#008000] h-[60px] w-[60px] border-[#007000] border-[1px]">
        <ShoppingCartIcon color="gold" strokeWidth={2.5}  className={`ml-[-4px]`} width={20}/>
        </Button>
      </div>
      </div>
      <div className="w-[90%] m-auto h-[] mt-[50px]">
      <Swiper
        pagination={{
          type: 'progressbar',
        }}
        navigation={true}
        modules={[Pagination, Navigation]}
        className="mySwiper"
      >
        <SwiperSlide>
          <img src={s1} alt="" />
        </SwiperSlide>
        <SwiperSlide>
        <img src={s2} alt="" />
        </SwiperSlide>
        <SwiperSlide>
        <img src={s3} alt="" />
        </SwiperSlide>
        <SwiperSlide>
        <img src={s4} alt="" />
        </SwiperSlide>
        <SwiperSlide>
        <img src={s5} alt="" />
        </SwiperSlide>
        <SwiperSlide>
        <img src={s6} alt="" />
        </SwiperSlide>
        <SwiperSlide>
        <img src={s7} alt="" />
        </SwiperSlide>
        <SwiperSlide>
        <img src={s8} alt="" />
        </SwiperSlide>
        <SwiperSlide>
        <img src={s9} alt="" />
        </SwiperSlide>
        <SwiperSlide>
        <img src={s10} alt="" />
        </SwiperSlide>
      </Swiper>
      </div>

      <h2 className="text-center text-[50px] hover:text-black text-[#f00] mt-[100px]">Популярный Категории </h2>
      <div className="h-[100vh] mt-[50px] grid grid-cols-5 ml-[200px]">
        <div>1</div>
        <div>2</div>
        <div>3</div>
        <div>4</div>
        <div>5</div>
        <div>6</div>
        <div>7</div>
        <div>8</div>
        <div>9</div>
        <div>10</div>
        <div>11</div>
        <div>12</div>
        <div>13</div>
        <div>14</div>
        <div>15</div>
        <div>16</div>
        <div>17</div>
        <div>18</div>
        <div>19</div>
        <div>20</div>
      </div>
      <div className="m-auto w-[95%] mb-[150px]">
        <h1 className="text-center text-black text-[50px] hover:text-red-500">Самые горячие скидки🔥</h1>
        <div className="flex justify-center ml-[0px] h-[250px] w-[100%] mt-[50px]">
        <Swiper
        slidesPerView={3}
        grid={{
          rows: 2,
        }}
        spaceBetween={30}
        pagination={{
          clickable: true,
        }}
        modules={[Grid, Pagination]}
        className="mySwiper2"
      >
        <SwiperSlide>Slide 1</SwiperSlide>
        <SwiperSlide>Slide 2</SwiperSlide>
        <SwiperSlide>Slide 3</SwiperSlide>
        <SwiperSlide>Slide 4</SwiperSlide>
        <SwiperSlide>Slide 5</SwiperSlide>
        <SwiperSlide>Slide 6</SwiperSlide>
        <SwiperSlide>Slide 7</SwiperSlide>
        <SwiperSlide>Slide 8</SwiperSlide>
        <SwiperSlide>Slide 9</SwiperSlide>
      </Swiper>
        </div>
      </div>

      <div>
      <div className="w-[95%] m-auto">
          <h1 className="text-center text-[50px] hover:text-red-500">Рекоменуюмый товары</h1>
          <div className="h-[300px]">
          <Swiper
        slidesPerView={3}
        grid={{
          rows: 2,
        }}
        spaceBetween={30}
        pagination={{
          clickable: true,
        }}
        modules={[Grid, Pagination]}
        className="mySwiper2"
      >
        <SwiperSlide>Slide 1</SwiperSlide>
        <SwiperSlide>Slide 2</SwiperSlide>
        <SwiperSlide>Slide 3</SwiperSlide>
        <SwiperSlide>Slide 4</SwiperSlide>
        <SwiperSlide>Slide 5</SwiperSlide>
        <SwiperSlide>Slide 6</SwiperSlide>
        <SwiperSlide>Slide 7</SwiperSlide>
        <SwiperSlide>Slide 8</SwiperSlide>
        <SwiperSlide>Slide 9</SwiperSlide>
      </Swiper>
          </div>
        </div>
      <div className="w-[95%] m-auto mt-[100px]">
          <h1 className="text-center hover:text-red-500 text-[50px] cursor-pointer">
          <a href="https://topfile.tj/top100.html">Популярный товары</a></h1>
          <div className="h-[300px]">
          <Swiper
        slidesPerView={3}
        grid={{
          rows: 2,
        }}
        spaceBetween={30}
        pagination={{
          clickable: true,
        }}
        modules={[Grid, Pagination]}
        className="mySwiper2"
      >
        <SwiperSlide>Slide 1</SwiperSlide>
        <SwiperSlide>Slide 2</SwiperSlide>
        <SwiperSlide>Slide 3</SwiperSlide>
        <SwiperSlide>Slide 4</SwiperSlide>
        <SwiperSlide>Slide 5</SwiperSlide>
        <SwiperSlide>Slide 6</SwiperSlide>
        <SwiperSlide>Slide 7</SwiperSlide>
        <SwiperSlide>Slide 8</SwiperSlide>
        <SwiperSlide>Slide 9</SwiperSlide>
      </Swiper>
          </div>
        </div>

        <div className="w-[95%] m-auto mt-[50px]">
          <h1 className="text-center hover:text-red-500 text-[50px] cursor-pointer">Хиты Года</h1>
          <div className="h-[300px]"> 
          <Swiper
        slidesPerView={3}
        grid={{
          rows: 2,
        }}
        spaceBetween={30}
        pagination={{
          clickable: true,
        }}
        modules={[Grid, Pagination]}
        className="mySwiper2"
      >
        <SwiperSlide>Slide 1</SwiperSlide>
        <SwiperSlide>Slide 2</SwiperSlide>
        <SwiperSlide>Slide 3</SwiperSlide>
        <SwiperSlide>Slide 4</SwiperSlide>
        <SwiperSlide>Slide 5</SwiperSlide>
        <SwiperSlide>Slide 6</SwiperSlide>
        <SwiperSlide>Slide 7</SwiperSlide>
        <SwiperSlide>Slide 8</SwiperSlide>
        <SwiperSlide>Slide 9</SwiperSlide>
      </Swiper>
          </div>
        </div>
        <div className="w-[95%]  m-auto mt-[50px]">
          <h1 className="text-center hover:text-red-500 text-[50px] cursor-pointer">Хиты Месеца</h1>
          <div className="h-[300px]">
          <Swiper
        slidesPerView={3}
        grid={{
          rows: 2,
        }}
        spaceBetween={30}
        pagination={{
          clickable: true,
        }}
        modules={[Grid, Pagination]}
        className="mySwiper2"
      >
        <SwiperSlide>Slide 1</SwiperSlide>
        <SwiperSlide>Slide 2</SwiperSlide>
        <SwiperSlide>Slide 3</SwiperSlide>
        <SwiperSlide>Slide 4</SwiperSlide>
        <SwiperSlide>Slide 5</SwiperSlide>
        <SwiperSlide>Slide 6</SwiperSlide>
        <SwiperSlide>Slide 7</SwiperSlide>
        <SwiperSlide>Slide 8</SwiperSlide>
        <SwiperSlide>Slide 9</SwiperSlide>
      </Swiper>
          </div>
        </div>
        <div className="mt-[50px] w-[95%] m-auto">
          <h1 className="text-center hover:text-red-500 text-[50px] cursor-pointer">Хиты Дня</h1>
          <div className="h-[300px]"> 
          <Swiper
        slidesPerView={3}
        grid={{
          rows: 2,
        }}
        spaceBetween={30}
        pagination={{
          clickable: true,
        }}
        modules={[Grid, Pagination]}
        className="mySwiper2"
      >
        <SwiperSlide>Slide 1</SwiperSlide>
        <SwiperSlide>Slide 2</SwiperSlide>
        <SwiperSlide>Slide 3</SwiperSlide>
        <SwiperSlide>Slide 4</SwiperSlide>
        <SwiperSlide>Slide 5</SwiperSlide>
        <SwiperSlide>Slide 6</SwiperSlide>
        <SwiperSlide>Slide 7</SwiperSlide>
        <SwiperSlide>Slide 8</SwiperSlide>
        <SwiperSlide>Slide 9</SwiperSlide>
      </Swiper>
          </div>
        </div>
      </div>
    </div>

  )
}

export default App