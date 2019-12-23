int main(int argc, char argv) {
    xmlDocPtr doc;
    xmlXPathContextPtr ctxt;
    xmlXPathObjectPtr xpathRes;
 
    if (2 != argc) {
        fprintf(stderr, usage %s [FICHIER � PARSER]n, argv[0]);
        return EXIT_FAILURE;
    }
    if (NULL == (doc = xmlParseFile(argv[1]))) {
        fprintf(stderr, �chec de xmlParseFile()n);
        return EXIT_FAILURE;
    }
 
    xmlXPathInit();
    if (NULL == (ctxt = xmlXPathNewContext(doc))) {
        fprintf(stderr, Erreur lors de la cr�ation du contexte XPathn);
        return EXIT_FAILURE;
    }
    if (NULL == (xpathRes = xmlXPathEvalExpression(BAD_CAST message[@index = '1'], ctxt))) {
        fprintf(stderr, Erreur sur l'expression XPathn);
        return EXIT_FAILURE;
    }
    if (XPATH_NODESET == xpathRes-type) {
        int i;
        xmlBufferPtr buf;
        if (NULL == (buf = xmlBufferCreate())) {
            fprintf(stderr, �chec cr�ation du buffern);
            return EXIT_FAILURE;
        }
        if (0 == xpathRes-nodesetval-nodeNr) {
            printf(Aucune correspondancen);
        } else {
            for (i = 0; i  xpathRes-nodesetval-nodeNr; i++) {
                const xmlChar content;
                xmlNodePtr n = xpathRes-nodesetval-nodeTab[i];
 
                xmlBufferEmpty(buf);
                xmlNodeDump(buf, doc, n, 0  Niveau de d�marrage de l'indentation , 1  0 pour aucun formatage );
                content = xmlBufferContent(buf);
                printf(Contenu trouv�  %sn, content);
            }
            xmlBufferFree(buf);
        }
    }
    xmlXPathFreeObject(xpathRes);
    xmlXPathFreeContext(ctxt);
    xmlFreeDoc(doc);
 
    return EXIT_SUCCESS;
}