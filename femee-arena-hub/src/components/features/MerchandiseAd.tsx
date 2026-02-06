import { ShoppingBag } from "lucide-react";
import { Card } from "../ui/card";
import { Button } from "../ui/button";
import merchandiseImage from "@/assets/merchandise.jpg";

const MerchandiseAd = () => {
  return (
    <Card className="bg-card border-border overflow-hidden">
      <div className="p-4 space-y-4">
        <div className="flex items-center gap-2 pb-3 border-b border-border">
          <ShoppingBag className="h-5 w-5 text-primary" />
          <h2 className="text-lg font-bold text-foreground">Loja FEMEE</h2>
        </div>
        
        <div className="aspect-square overflow-hidden rounded-lg">
          <img 
            src={merchandiseImage}
            alt="Produtos FEMEE"
            className="w-full h-full object-cover"
          />
        </div>
        
        <div className="space-y-3">
          <h3 className="font-bold text-foreground">
            Produtos Oficiais FEMEE
          </h3>
          <p className="text-sm text-muted-foreground">
            Camisas oficiais, pulseiras, stickers e muito mais. Mostre seu apoio aos esportes eletrônicos de Minas Gerais!
          </p>
          
          <div className="space-y-2 pt-2">
            <div className="flex items-center justify-between text-sm">
              <span className="text-muted-foreground">Camisas Oficiais</span>
              <span className="font-semibold text-foreground">R$ 89,90</span>
            </div>
            <div className="flex items-center justify-between text-sm">
              <span className="text-muted-foreground">Pulseiras</span>
              <span className="font-semibold text-foreground">R$ 19,90</span>
            </div>
            <div className="flex items-center justify-between text-sm">
              <span className="text-muted-foreground">Pack Stickers</span>
              <span className="font-semibold text-foreground">R$ 14,90</span>
            </div>
          </div>
          
          <Button className="w-full esports-glow mt-4">
            Visitar Loja
          </Button>
        </div>
      </div>
    </Card>
  );
};

export default MerchandiseAd;
