using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faropets.Library
{
    public class LibPaginador<T>
    {
        //cantidad de resultados por páginas.
        private int pagi_cuantos = 10;
        //cantidad de enlaces que se mostrarán como máximo en la barra de navegación.
        private int pagi_nav_num_enlaces = 3;
        private int pagi_actual;
        //definimos que irá en el enlace a la página anterior.
        private String pagi_nav_anterior = " &laquo; Anterior ";
        //definimos que irá en el enlace de la página siguiente.
        private String pagi_nav_siguiente = "Siguiente &raquo;";
        //definimos que irá en el enlace de la página siguiente.
        private String pagi_nav_primera = " &laquo Primero";
        private String pagi_nav_ultima = "Ultimo &raquo";
        private String pagi_navegacion = null;

        public object[] paginador(List<T> table, int pagina, int registros ,String area, String 
            controller, String action, String host) {

            pagi_actual = pagina == 0 ? 1 : pagina;
            if (registros > 0)
            {
                pagi_cuantos = registros;
            }

            int pagi_total_reg = table.Count;
            double valor1 = Math.Ceiling((double)pagi_total_reg / (double)pagi_cuantos);
            int pagi_totalPags = Convert.ToUInt16(Math.Ceiling(valor1));
            if ( pagi_actual != 1)
            {
                //Si no estamos en la página 1. Ponemos el Enlace "Primera".
                int pagi_url = 1; // sera el número de páginas al que enlazamos.
                pagi_navegacion += "<a class='btn btn-default' href'" + host + "/" + controller + "/" + action + "?ID=" + pagi_url +
                    "&Registros=" + pagi_cuantos + "&area=" + area + "'>" + pagi_nav_primera + "</a>";

                //Si no estamos en la página 1. Ponemos el Enlace "Anterior".
                 pagi_url = pagi_actual -1; // sera el número de páginas al que enlazamos.
                pagi_navegacion += "<a class='btn btn-defaults' href'" + host + "/" + controller + "/" + action + "?ID=" + pagi_url + 
                    "&Registros=" + pagi_cuantos +  "&area=" + area + "'>" + pagi_nav_anterior + "</a>";

            }
            //si se definió la variable pagi_nav_num_enlace
            //Calcularemos los intervalos para restar y sumar apartir de la página actual.
            double valor2 = (pagi_nav_num_enlaces / 2);
            int pagi_nav_intervalo = Convert.ToInt16(Math.Round(valor2));
            //se calculara desde que número de página se mostrará
            int pagi_nav_desde = pagi_actual - pagi_nav_intervalo;
            //calculamos hasta qué número de página de mostrará.
            int pagi_nav_hasta = pagi_actual + pagi_nav_intervalo;
            //si pagi_nav_desde es un número negativo
            if (pagi_nav_desde < 1)
            {
                //le sumamos la cantidad sobrante al final para mantener
                //el número de enlacres que se quiere mostrar.
                pagi_nav_hasta -= (pagi_nav_desde - 1);
                //establecemos pagi_nav_desde como 1
                pagi_nav_desde = 1;
            }
            //Si pagi_nav_hasta es un número mayor que el total de las páginas.
            if (pagi_nav_hasta > pagi_totalPags)
            {
                //le restamos la cantidad excesita al comienzo para mantener.
                //el número de enlaces que se quiere mostrar.
                pagi_nav_desde -= (pagi_nav_hasta - pagi_totalPags);
                //establecemos el pagi_nav_hasta como el total de las páginas.
                pagi_nav_hasta = pagi_totalPags;
                //hacemos el último ajuste verificando que al cambiar pagi_nav_desde
                //no haya quedado con un valor no valido
                if (pagi_nav_desde < 1)
                {
                    pagi_nav_desde = 1;
                }

            }
            for (int pagi_i = pagi_nav_desde; pagi_i <= pagi_nav_hasta; pagi_i++)
            {
                //Desde página 1 hasta última página (pagi_totalPags)
                if (pagi_i == pagi_actual)
                {
                    //si el número de página es la actual (pagi_actual) se escribe el número, pero sin enlace y en negrita
                    pagi_navegacion += "<span class='btn btn-default' disabled='disabled'>" + pagi_i + "</span>";
                }
                else
                {
                    //Si es cualquier otro. Se escribe el enlace a dicho número de página.
                    pagi_navegacion += "<a class='btn btn-defaults' href'" + host + "/" + controller + "/" + action + "?ID=" + pagi_i +
                    "&Registros=" + pagi_cuantos + "&area=" + area + "'>" + pagi_i + "</a>";

                }

            }

            if (pagi_actual < pagi_totalPags)
            {
                //Si estamos en la última página ponemos el enlace "Siguiente"
                int pagi_url = pagi_actual + 1; //Será el número de página al que enlazamos.
                pagi_navegacion += "<a class='btn btn-defaults' href'" + host + "/" + controller + "/" + action + "?ID=" + pagi_url +
                   "&Registros=" + pagi_cuantos + "&area=" + area + "'>" + pagi_nav_siguiente + "</a>";

                //Si no estamos en la última página se pone el enlace "Última"
                 pagi_url = pagi_actual + 1; //Será el número de página al que enlazamos.
                pagi_navegacion += "<a class='btn btn-defaults' href'" + host + "/" + controller + "/" + action + "?ID=" + pagi_url +
                   "&Registros=" + pagi_cuantos + "&area=" + area + "'>" + pagi_nav_ultima + "</a>";

            }
            //obtención de los registros que se mostrarán en la página actual
            /* -----------------------------------------------------------------*/
            //Calculamos desde que registro se mostrará la página.
            int pagi_inicial = (pagi_actual - 1) * pagi_cuantos;
            var query = table.Skip(pagi_inicial).Take(pagi_cuantos).ToList();

            // Generación de la información de los registros mostrados.

            //Número de registros de la página actual
            int pagi_desde = pagi_inicial + 1;
            //Número del último registro de la  página actual
            int pagi_hasta = pagi_inicial + pagi_cuantos;
            if (pagi_hasta > pagi_total_reg)
            {
                //si estamos en la última página 
                //el último registr de la página actual será igual al número de registros que hay en el sistema.
                pagi_nav_hasta = pagi_total_reg;
            }


            String pagi_info = "del <b" + pagi_actual + "</b> al <b>" + pagi_totalPags + "</b> de <b>"
                + pagi_total_reg + "</b> <b> /" + pagi_cuantos + "</b>";
            object[] data = { pagi_info, pagi_navegacion, query };
            return data;

        }

    }
}
