import { ChevronUpIcon } from "@heroicons/react/24/solid";
import {
  Menu,
  MenuHandler,
  MenuList,
  MenuItem,
} from "@material-tailwind/react";

{/*Да даруни Барои > иконка 180о Тов хурдан className={`h-3.5 w-3.5 transition-transform ${nowOpenMenu ? "rotate-180" : ""}`} */}
{/*Да даруни Menuhandler мемонем барои мену пойон фаромадан ${openMenuTop ? `mt-[${arr.length * 40}px]`: `mt-[0px]` */}


const Catalog = ({ obj }) => {
  return (
    <div>
      {obj.map((e) => {
        return (
            <Menu key={e.catalogName} placement="bottom" offset={0}>
            <MenuHandler className={`flex items-center font-bold justify-between`}>
              <MenuItem>{e.catalogName}<ChevronUpIcon strokeWidth={2.5}  className={`h-3.5 w-3.5`}/> </MenuItem>
            </MenuHandler>
            <MenuList>
              {e.children.map((e) => {
                return (
                  <Menu key={e.name} placement="left" offset={0}>
                    <MenuHandler className={ `flex items-center font-bold justify-between`}>
                      <MenuItem>{e.name}<ChevronUpIcon strokeWidth={2.5} className={`h-3.5 w-3.5`}/></MenuItem>
                    </MenuHandler>
                    <MenuList>
                      {e.children.map((e) => {
                        return <MenuItem key={e}>{e}</MenuItem>;
                      })}
                    </MenuList>
                  </Menu>
                );
              })}
            </MenuList>
          </Menu>
        );
      })}
    </div>
  );
};

export default Catalog;
